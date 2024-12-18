
namespace RoslynPythonCoreParser;

public partial class PythonCoreParser
{
    /// <summary>
    ///  Handling grammar rule: test [ ':=' test ]
    /// </summary>
    /// <returns> NamedExprNode | ExprNode </returns>
    private ExprNode ParseNamedExpr()
    {
        var pos = Lexer.Position;
        var left = ParseExpr();

        if (Lexer.Symbol is not ColonAssignToken) return left;
        
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseExpr();

        return new NamedExprNode(pos, Lexer.Position, left, symbol, right);
    }

    /// <summary>
    ///  Handling grammar rule: or_test [ 'if' or_test 'else' test ] | LambdaDef
    /// </summary>
    /// <returns> TestExprNode | ExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseTest()
    {
        if (Lexer.Symbol is LambdaToken) return ParseLambdaDef(true);

        var pos = Lexer.Position;
        var left = ParseOrTest();

        if (Lexer.Symbol is not IfToken) return left;

        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseOrTest();

        if (Lexer.Symbol is not ElseToken) throw new Exception();

        var symbol2 = Lexer.Symbol;
        Lexer.Advance();
        
        var next = ParseTest();

        return new TestExprNode(pos, Lexer.Position, left, symbol1, right, symbol2, next);
    }

    /// <summary>
    ///  Handle grammar rule: or_test | lambdef_nocond
    /// </summary>
    /// <returns> ExprNode </returns>
    private ExprNode ParseTestNoCond()
    {
        return Lexer.Symbol is LambdaToken ? ParseLambdaDef(false) : ParseOrTest();
    }
    
    /// <summary>
    ///  Handle grammar rule: ( 'lambda' [varargslist] ':' test ) | ( 'lambda' [varargslist] ':' test_nocond )
    /// </summary>
    /// <param name="isConditional"></param>
    /// <returns> LambdaExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseLambdaDef(bool isConditional)
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        var left = Lexer.Symbol is not ColonToken ? ParseVarArgsList() : null;

        if (Lexer.Symbol is not ColonToken) throw new Exception();
        var symbol2 = Lexer.Symbol;
        Lexer.Advance();

        var right = isConditional ? ParseTest() : ParseTestNoCond();

        return new LambdaExprNode(pos, Lexer.Position, symbol1, left, symbol2, right, isConditional);
    }
    
    /// <summary>
    ///  Handling grammar rule: AndTest ( 'or' AndTest )*
    /// </summary>
    /// <returns> OrTestExprNode | ExprNode </returns>
    private ExprNode ParseOrTest()
    {
        var pos = Lexer.Position;
        var left = ParseAndTest();

        while (Lexer.Symbol is OrToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseAndTest();

            left = new OrTestExprNode(pos, Lexer.Position, left, symbol, right);
        }

        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: NotTest ( 'and' NotTest )*
    /// </summary>
    /// <returns> AndTestExprNode | ExprNode </returns>
    private ExprNode ParseAndTest()
    {
        var pos = Lexer.Position;
        var left = ParseNotTest();

        while (Lexer.Symbol is OrToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseNotTest();

            left = new AndTestExprNode(pos, Lexer.Position, left, symbol, right);
        }

        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: Comparison | 'not' NotTest
    /// </summary>
    /// <returns> NotTestExprNode | ExprNode </returns>
    private ExprNode ParseNotTest()
    {
        if (Lexer.Symbol is not NotToken) return ParseComparison();
        
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseNotTest();

        return new NotTestExprNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handling grammar rule:  expr ( ('<' | '<=' | '>' | '>=' | '==' | '!=' | '<>' | 'in' | 'is' | 'is 'not' | 'not' 'in') expr )*
    /// </summary>
    /// <returns> CompareLessExprNode | CompareLessEqualExprNode | CompareEqualExprNode | CompareNotInExprNode |
    /// CompareGreaterExprNode | CompareGreaterEqualExprNode | CompareInExprNode | CompareNotInExprNode |
    /// CompareIsExprNode | CompareIsNotExprNode | ExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseComparison()
    {
        var pos = Lexer.Position;
        var left = ParseExpr();

        while (Lexer.Symbol is CompareLessToken 
               or CompareLessEqualToken 
               or CompareGreaterToken 
               or CompareLessEqualToken
               or CompareEqualToken
               or CompareNotEqualToken
               or InToken
               or IsToken
               or NotToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();
            
            if (symbol is IsToken)
            {
                if (Lexer.Symbol is NotToken)
                {
                    var symbol2 = Lexer.Symbol;
                    Lexer.Advance();

                    var right = ParseExpr();

                    left = new CompareIsNotExprNode(pos, Lexer.Position, left, symbol, symbol2, right);
                }
                else
                {
                    var right = ParseExpr();
                    left = new CompareIsExprNode(pos, Lexer.Position, left, symbol, right);
                }
            }
            else if (symbol is NotToken)
            {
                if (Lexer.Symbol is not InToken) throw new Exception();

                var symbol2 = Lexer.Symbol;
                Lexer.Advance();
                var right = ParseExpr();

                left = new CompareNotInExprNode(pos, Lexer.Position, left, symbol, symbol2, right);
            }
            else
            {
                var right = ParseExpr();

                left = symbol switch
                {
                    CompareLessToken => new CompareLessExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareLessEqualToken => new CompareLessEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareEqualToken => new CompareEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareGreaterEqualToken => new CompareGreaterEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareGreaterToken => new CompareGreaterExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareNotEqualToken => new CompareNotEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    InToken => new CompareInExprNode(pos, Lexer.Position, left, symbol, right),
                    _ => throw new Exception()
                };
            }
        }

        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: '*' Expr
    /// </summary>
    /// <returns> StarExprNode </returns>
    private ExprNode ParseStarExpr()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseExpr();

        return new StarExprNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handler grammar rule: XorExpr ( '|' XorExpr )*
    /// </summary>
    /// <returns> OrExprNode | ExprNode </returns>
    private ExprNode ParseExpr()
    {
        var pos = Lexer.Position;
        var left = ParseXorExpr();

        while (Lexer.Symbol is BinaryOperatorBitOrToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseXorExpr();

            left = new OrExprNode(pos, Lexer.Position, left, symbol, right);
        }
        
        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: AndExpr ( '^' AndExpr )*
    /// </summary>
    /// <returns></returns>
    private ExprNode ParseXorExpr()
    {
        var pos = Lexer.Position;
        var left = ParseAndExpr();

        while (Lexer.Symbol is BinaryOperatorBitXorToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseAndExpr();

            left = new XorExprNode(pos, Lexer.Position, left, symbol, right);
        }
        
        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: ShiftExpr ( '&' ShiftExpr )*
    /// </summary>
    /// <returns></returns>
    private ExprNode ParseAndExpr()
    {
        var pos = Lexer.Position;
        var left = ParseShiftExpr();

        while (Lexer.Symbol is BinaryOperatorBitAndToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseShiftExpr();

            left = new AndExprNode(pos, Lexer.Position, left, symbol, right);
        }
        
        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: ArithExpr ( ( '<<' | '>>' ) ArithExpr )*
    /// </summary>
    /// <returns> ShiftLeftExprNode | ShiftRightExprNode | ExprNode </returns>
    private ExprNode ParseShiftExpr()
    {
        var pos = Lexer.Position;
        var left = ParseArithExpr();

        while (Lexer.Symbol is BinaryOperatorShiftLeftToken or BinaryOperatorShiftRightToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseArithExpr();

            left = symbol switch
            {
                BinaryOperatorShiftLeftToken => new ShiftLeftExprNode(pos, Lexer.Position, left, symbol, right),
                _ => new ShiftRightExprNode(pos, Lexer.Position, left, symbol, right)
            };
        }

        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: Term ( ( '+' | '-' ) Term )*
    /// </summary>
    /// <returns> BinaryOperatorPlusExprNode | BinaryOperatorMinusExprNode | ExprNode </returns>
    private ExprNode ParseArithExpr()
    {
        var pos = Lexer.Position;
        var left = ParseTerm();

        while (Lexer.Symbol is BinaryOperatorPlusToken or BinaryOperatorMinusToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseTerm();

            left = symbol switch
            {
                BinaryOperatorPlusToken => new BinaryOperatorPlusExprNode(pos, Lexer.Position, left, symbol, right),
                _ => new BinaryOperatorMinusExprNode(pos, Lexer.Position, left, symbol, right)
            };
        }

        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: Factor ( ( '*' | '@' | '/' | '%' | '//' ) Factor )*
    /// </summary>
    /// <returns></returns>
    private ExprNode ParseTerm()
    {
        var pos = Lexer.Position;
        var left = ParseFactor();

        while (Lexer.Symbol is BinaryOperatorMulToken 
               or BinaryOperatorMatricesToken
               or BinaryOperatorDivToken
               or BinaryOperatorModuloToken
               or BinaryOperatorFloorDivToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseFactor();

            left = symbol switch
            {
                BinaryOperatorMulToken => new BinaryOperatorMulExprNode(pos, Lexer.Position, left, symbol, right),
                BinaryOperatorMatricesToken => new BinaryOperatorMatriceExprNode(pos, Lexer.Position, left, symbol, right),
                BinaryOperatorDivToken => new BinaryOperatorDivExprNode(pos, Lexer.Position, left, symbol, right),
                BinaryOperatorModuloToken => new BinaryOperatorModuloExprNode(pos, Lexer.Position, left, symbol, right),
                _ => new BinaryOperatorFloorDivExprNode(pos, Lexer.Position, left, symbol, right)
            };
        }

        return left;
    }
    
    /// <summary>
    ///  Handling grammar rule: ( '+' | '-' | '~' ) factor | Power
    /// </summary>
    /// <returns> UnaryOperatorPlusExprNode | UnaryOperatorMinusExprNode | UnaryOperatorInvertExprNode | ExprNode </returns>
    private ExprNode ParseFactor()
    {
        var pos = Lexer.Position;

        if (Lexer.Symbol is BinaryOperatorPlusToken or BinaryOperatorMinusToken or UnaryOperatorBitInvertToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseFactor();

            return symbol switch
            {
                BinaryOperatorPlusToken => new UnaryOperatorPlusExprNode(pos, Lexer.Position, symbol, right),
                BinaryOperatorMinusToken => new UnaryOperatorMinusExprNode(pos, Lexer.Position, symbol, right),
                _ => new UnaryOperatorInvertExprNode(pos, Lexer.Position, symbol, right),
            };
        }

        return ParsePower();
    }
    
    /// <summary>
    ///  Handling grammar rule: AtomExpr [ '**' Factor ]
    /// </summary>
    /// <returns> BinaryOperatorPowerExprNode | ExprNode </returns>
    private ExprNode ParsePower()
    {
        var pos = Lexer.Position;
        var left = ParseAtomExpr();

        if (Lexer.Symbol is BinaryOperatorPowerToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();
            var right = ParseFactor();

            return new BinaryOperatorPowerExprNode(pos, Lexer.Position, left, symbol, right);
        }
        
        return left;
    }
    
    /// <summary>
    ///  Handle grammar rule: [ AWAIT ] Atom Trailer*
    /// </summary>
    /// <returns> AtomExprNode | ExprNode </returns>
    private ExprNode ParseAtomExpr()
    {
        var pos = Lexer.Position;
        Token? await = null;

        if (Lexer.Symbol is AwaitToken)
        {
            await = Lexer.Symbol;
            Lexer.Advance();
        }

        var right = ParseAtom();

        if (await != null || Lexer.Symbol is LeftParenToken || Lexer.Symbol is LeftBracketToken ||
            Lexer.Symbol is DotToken)
        {
            if (Lexer.Symbol is LeftParenToken || Lexer.Symbol is LeftBracketToken ||
                Lexer.Symbol is DotToken)
            {
                List<ExprNode> trailers = new List<ExprNode>();

                while (Lexer.Symbol is LeftParenToken || Lexer.Symbol is LeftBracketToken ||
                       Lexer.Symbol is DotToken)
                {
                    trailers.Add(ParseTrailer());
                }
                
                return new AtomExprNode(pos, Lexer.Position, await, right, trailers.ToArray());
            }

            return new AtomExprNode(pos, Lexer.Position, await, right, []);
        }

        return right;
    }
    
    /// <summary>
    ///  Handling grammar rule: ('(' [yield_expr|testlist_comp] ')' |
    /// '[' [testlist_comp] ']' |
    /// '{' [dictorsetmaker] '}' |
    /// NAME | NUMBER | STRING+ | '...' | 'None' | 'True' | 'False')
    /// </summary>
    /// <returns> LiteralNameExprNode | LiteralNumberExprNode | LiteralStringExprNode | LiteralElipsisExprNode |
    /// LiteralNoneExprNode | LiteralTrueExprNode | LiteralFalseExprNode | LiteralTupleExprNode |
    /// LiteralListExprNode | LiteralDictionaryExprNode | LiteralSetExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseAtom()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        if (symbol is StringToken)
        {
            var nodes = new List<StringToken>();

            while (Lexer.Symbol is StringToken token)
            {
                nodes.Add(token);
                Lexer.Advance();
            }
            
            return new LiteralStringExprNode(pos, Lexer.Position, nodes.ToArray());
        }
        
        if (symbol is LeftParenToken)
        {
            ExprNode? right = Lexer.Symbol switch
            {
                YieldToken => ParseYieldExpr(),
                RightParenToken => null,
                _ => ParseTestListComp()
            };
            
            if (Lexer.Symbol is not RightParenToken) throw new Exception();

            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new LiteralTupleExprNode(pos, Lexer.Position, symbol, right, symbol2);
        }

        if (symbol is LeftBracketToken)
        {
            ExprNode? right = Lexer.Symbol switch
            {
                RightParenToken => null,
                _ => ParseTestListComp()
            };
            
            if (Lexer.Symbol is not RightBracketToken) throw new Exception();

            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new LiteralListExprNode(pos, Lexer.Position, symbol, right, symbol2);
        }

        if (symbol is LeftCurlyToken symbol1)
        {
            return ParseDictorSetMaker(symbol1, pos);
        }

        return symbol switch
        {
            NameToken => new LiteralNameExprNode(pos, Lexer.Position, symbol),
            NumberToken => new LiteralNumberExprNode(pos, Lexer.Position, symbol),
            NoneToken => new LiteralNoneExprNode(pos, Lexer.Position, symbol),
            TrueToken => new LiteralTrueExprNode(pos, Lexer.Position, symbol),
            FalseToken => new LiteralFalseExprNode(pos, Lexer.Position, symbol),
            ElipsisToken => new LiteralEllipsisExprNode(pos, Lexer.Position, symbol),
            _ => throw new Exception()
        };
    }
    
    /// <summary>
    ///  Handle grammar rule: (namedexpr_test|star_expr) ( comp_for | (',' (namedexpr_test|star_expr))* [','] )
    /// </summary>
    /// <returns> TestListCompExprNode | ExprNode </returns>
    private ExprNode ParseTestListComp()
    {
        var pos = Lexer.Position;
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        
        nodes.Add( Lexer.Symbol switch
        {
            BinaryOperatorMulToken => ParseStarExpr(),
            _ => ParseNamedExpr()
        });

        if (Lexer.Symbol is ForToken or AsyncToken)
        {
            nodes.Add(ParseCompFor());
        }
        else
        {
            while (Lexer.Symbol is CommaToken)
            {
                separators.Add(Lexer.Symbol);
                Lexer.Advance();

                if (Lexer.Symbol is RightParenToken or RightBracketToken) break;
                
                nodes.Add( Lexer.Symbol switch
                {
                    BinaryOperatorMulToken => ParseStarExpr(),
                    _ => ParseNamedExpr()
                });
            }
        }

        return nodes.Count == 1
            ? nodes[0]
            : new TestListCompExprNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    private ExprNode ParseTrailer()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        if (symbol is LeftParenToken)
        {
            var right = Lexer.Symbol is RightParenToken ? null : ParseArgList();

            if (Lexer.Symbol is not RightParenToken) throw new Exception();
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new TrailerCallExprNode(pos, Lexer.Position, symbol, right, symbol2);
        }
        else if (symbol is LeftBracketToken)
        {
            var right = ParseSubscriptList();

            if (Lexer.Symbol is not RightBracketToken) throw new Exception();
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new TrailerIndexExprNode(pos, Lexer.Position, symbol, right, symbol2);
        }
        else if (symbol is DotToken)
        {
            if (Lexer.Symbol is not NameToken) throw new Exception();

            var symbol2 = Lexer.Symbol;
            Lexer.Advance();
            
            var right = new LiteralNameExprNode(pos, Lexer.Position, symbol2);

            return new TrailerDotNameExprNode(pos, Lexer.Position, symbol, right);
        }

        throw new Exception();
    }
    
    /// <summary>
    ///  Handling grammar rule: subscript (',' subscript)* [',']
    /// </summary>
    /// <returns> SubscriptListExprNode | ExprNode </returns>
    private ExprNode ParseSubscriptList()
    {
        var pos = Lexer.Position;
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        
        nodes.Add(ParseSubscript());

        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is RightBracketToken) break;
            
            nodes.Add(ParseSubscript());
        }
        
        return nodes.Count == 1
            ? nodes[0]
            : new SubscriptListExprNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }

    /// <summary>
    ///  Handling grammar rule: test | [test] ':' [test] [ ':' [Test]]
    /// </summary>
    /// <returns> SubscriptExprNode </returns>
    private ExprNode ParseSubscript()
    {
        var pos = Lexer.Position;
        var first = Lexer.Symbol is not ColonToken ? ParseTest() : null;
        
        Token? symbol1 = null, symbol2 = null;
        ExprNode? second = null, third = null;

        if (Lexer.Symbol is ColonToken)
        {
            symbol1 = Lexer.Symbol;
            Lexer.Advance();

            second = Lexer.Symbol is not ColonToken && Lexer.Symbol is not RightBracketToken && Lexer.Symbol is not CommaToken ? ParseTest() : null;

            if (Lexer.Symbol is ColonToken)
            {
                symbol2 = Lexer.Symbol;
                Lexer.Advance();

                third = Lexer.Symbol is not RightBracketToken && Lexer.Symbol is not CommaToken ? ParseTest() : null;
            }
        }

        return new SubscriptExprNode(pos, Lexer.Position, first, symbol1, second, symbol2, third);
    }
    
    /// <summary>
    ///  Handle grammar rule: (expr|star_expr) (',' (expr|star_expr))* [',']
    /// </summary>
    /// <returns> ExprListExprNode | ExprNode </returns>
    private ExprNode ParseExprList()
    {
        var pos = Lexer.Position;
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        
        nodes.Add( Lexer.Symbol switch
        {
            BinaryOperatorMulToken => ParseStarExpr(),
            _ => ParseExpr()
        });
        
        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is InToken) break;
            
            nodes.Add( Lexer.Symbol switch
            {
                BinaryOperatorMulToken => ParseStarExpr(),
                _ => ParseExpr()
            });
        }
        
        return nodes.Count == 1
            ? nodes[0]
            : new ExprListExprNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: test (',' test)* [',']
    /// </summary>
    /// <returns> TestListExprNode | ExprNode </returns>
    private ExprNode ParseTestList()
    {
        var pos = Lexer.Position;
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        
        nodes.Add( ParseTest() );
        
        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is SemiColonToken or NewlineToken) break;
            
            nodes.Add( ParseTest() );
        }
        
        return nodes.Count == 1
            ? nodes[0]
            : new TestListExprNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: ( ((test ':' test | '**' expr)
    /// (comp_for | (',' (test ':' test | '**' expr))* [','])) |
    /// ((test | star_expr)
    /// (comp_for | (',' (test | star_expr))* [','])) )
    /// </summary>
    /// <param name="symbol1"></param>
    /// <param name="pos"></param>
    /// <returns> LiteralDictionaryExprNode | LiteralSetExprNode </returns>
    private ExprNode ParseDictorSetMaker(LeftCurlyToken symbol1, uint pos)
    {
        /* Empty dictionary */
        if (Lexer.Symbol is RightCurlyToken)
        {
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new LiteralDictionaryExprNode(pos, Lexer.Position, symbol1, [], symbol2);
        }

        var isSet = false;
        var dictionaryElements = new List<DictionaryEntryExprNode>();
        var setElements = new List<ExprNode>();
        var separators = new List<Token>();
        var pos2 = Lexer.Position;
        
        /* First element */
        switch (Lexer.Symbol)
        {
            case BinaryOperatorMulToken:
            {
                var symbol3 = Lexer.Symbol;
                Lexer.Advance();
                var right1 = ParseExpr();

                setElements.Add(new LiteralSetReferenceExprNode(pos2, Lexer.Position, symbol3, right1));
                break;
            }
            case BinaryOperatorPowerToken:
            {
                var symbol3 = Lexer.Symbol;
                Lexer.Advance();
                var right1 = ParseTest();

                setElements.Add(new DictionaryReferenceExprNode(pos2, Lexer.Position, symbol3, right1));
                break;
            }
            default:
            {
                var left = ParseTest();

                if (Lexer.Symbol is ColonToken)
                {
                    var symbol5 = Lexer.Symbol;
                    Lexer.Advance();
                    isSet = false;
                    var right = ParseTest();
                    
                    dictionaryElements.Add(new DictionaryEntryExprNode(pos2, Lexer.Position, left, symbol5, right));
                }
                else
                {
                    isSet = true;
                    setElements.Add(left);
                }
                break;
            }
        }

        /* Second element is 'for' or 'async' or rest of elements in dictionary or set */
        if (Lexer.Symbol is ForToken or AsyncToken)
        {
            var pos3 = Lexer.Position;
            
            if (isSet)
            {
                setElements.Add(ParseCompFor());
            }
            else
            {
                var right3 = ParseCompFor();
                dictionaryElements.Add(new DictionaryEntryExprNode(pos3, Lexer.Position, ParseCompFor(), null, null));
            }
        }
        else /* Rest of elements */
        {
            if (isSet)
            {
                while (Lexer.Symbol is CommaToken)
                {
                    separators.Add(Lexer.Symbol);
                    Lexer.Advance();

                    if (Lexer.Symbol is RightCurlyToken) break;

                    if (Lexer.Symbol is BinaryOperatorMulToken)
                    {
                        var pos5 = Lexer.Position;
                        var symbol6 = Lexer.Symbol;
                        Lexer.Advance();

                        var right6 = ParseExpr();
                        
                        setElements.Add(new LiteralSetReferenceExprNode(pos5, Lexer.Position, symbol6, right6));
                    }
                    else setElements.Add(ParseTest());
                }
            }
            else
            {
                while (Lexer.Symbol is CommaToken)
                {
                    separators.Add(Lexer.Symbol);
                    Lexer.Advance();

                    if (Lexer.Symbol is RightCurlyToken) break;

                    if (Lexer.Symbol is BinaryOperatorPowerToken)
                    {
                        var symbol3 = Lexer.Symbol;
                        Lexer.Advance();
                        var right1 = ParseTest();

                        setElements.Add(new DictionaryReferenceExprNode(pos2, Lexer.Position, symbol3, right1));
                    }
                    else
                    {
                        var left8 = ParseTest();

                        if (Lexer.Symbol is not ColonToken) throw new Exception();
                        var symbol8 = Lexer.Symbol;
                        Lexer.Advance();

                        var right8 = ParseTest();
                        
                        dictionaryElements.Add(new DictionaryEntryExprNode(pos2, Lexer.Position, left8, symbol8, right8));
                    }
                }
            }
        }
        
        var symbol4 = Lexer.Symbol;
        Lexer.Advance();

        return isSet
            ? new LiteralSetExprNode(pos, Lexer.Position, symbol1, setElements.ToArray(), symbol4)
            : new LiteralDictionaryExprNode(pos, Lexer.Position, symbol1, dictionaryElements.ToArray(), symbol4);
    }
    
    /// <summary>
    ///  Handle grammar rule: argument (',' argument)*  [',']
    /// </summary>
    /// <returns> ArgListExprNode | ExprNode </returns>
    private ExprNode ParseArgList()
    {
        var pos = Lexer.Position;
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        
        nodes.Add( ParseArgument() );
        
        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is RightParenToken) break;
            
            nodes.Add( ParseArgument() );
        }
        
        return nodes.Count == 1
            ? nodes[0]
            : new ArgListExprNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: ( NAME [comp_for] |
    /// NAME ':=' test |
    /// NAME '=' test |
    /// '**' test |
    /// '*' test )
    /// </summary>
    /// <returns> ArgumentExprNode | ExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseArgument()
    {
        var pos = Lexer.Position;

        if (Lexer.Symbol is BinaryOperatorMulToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            if (Lexer.Symbol is not NameToken) throw new Exception();

            var pos2 = Lexer.Position;
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new DictionaryReferenceExprNode(pos, Lexer.Position, symbol, new LiteralNameExprNode(pos2, Lexer.Position, symbol2));
        }
        else if (Lexer.Symbol is BinaryOperatorPowerToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            if (Lexer.Symbol is not NameToken) throw new Exception();

            var pos2 = Lexer.Position;
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            return new LiteralSetReferenceExprNode(pos, Lexer.Position, symbol, new LiteralNameExprNode(pos2, Lexer.Position, symbol2));
        }
        else if (Lexer.Symbol is NameToken symbol3)
        {
            Lexer.Advance();
            var left = new LiteralNameExprNode(pos, Lexer.Position, symbol3);

            if (Lexer.Symbol is AsyncToken or ForToken)
            {
                var right = ParseCompFor();

                return new ArgumentExprNode(pos, Lexer.Position, left, null, right);
            }
            else if (Lexer.Symbol is ColonAssignToken or AssignToken)
            {
                var symbol4 = Lexer.Symbol;
                Lexer.Advance();

                var right = ParseTest();

                return new ArgumentExprNode(pos, Lexer.Position, left, symbol4, right);
            }
            
            return left;
        }
        else throw new Exception();
    }
    
    /// <summary>
    ///  Handle grammar rule: comp_for | comp_if
    /// </summary>
    /// <returns> ExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseCompIter()
    {
        return Lexer.Symbol switch
        {
            IfToken => ParseCompIf(),
            AsyncToken => ParseCompFor(),
            ForToken => ParseSyncCompFor(),
            _ => throw new Exception()
        };
    }
    
    /// <summary>
    ///  Handle grammar rule: for' exprlist 'in' or_test [comp_iter]
    /// </summary>
    /// <returns> SyncCompForExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseSyncCompFor()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        var left = ParseExprList();

        if (Lexer.Symbol is not InToken) throw new Exception();
        var symbol2 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseOrTest();
        
        var next = Lexer.Symbol is ForToken or IfToken or AsyncToken ? ParseCompIter() : null;

        return new SyncCompForExprNode(pos, Lexer.Position, symbol1, left, symbol2, right, next);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'async' SyncCompFor
    /// </summary>
    /// <returns> CompForExprNode </returns>
    private ExprNode ParseCompFor()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseSyncCompFor();

        return new CompForExprNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'if' test_nocond [comp_iter]
    /// </summary>
    /// <returns> CompIfExprNode | ExprNode </returns>
    private ExprNode ParseCompIf()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseTestNoCond();

        var next = Lexer.Symbol is ForToken or IfToken or AsyncToken ? ParseCompIter() : null;

        return new CompIfExprNode(pos, Lexer.Position, symbol, right, next);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'yield' ( ( 'from' Test ) | TestListStarExpr ) 
    /// </summary>
    /// <returns> YieldFromExprNode | YieldExprNode </returns>
    private ExprNode ParseYieldExpr()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        if (Lexer.Symbol is FromToken)
        {
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseTest();

            return new YieldFromExprNode(pos, Lexer.Position, symbol1, symbol2, right);
        }

        var right2 = ParseTestListStarExpr();

        return new YieldExprNode(pos, Lexer.Position, symbol1, right2);
    }
}